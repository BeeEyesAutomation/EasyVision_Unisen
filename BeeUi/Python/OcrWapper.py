import easyocr
import re
import numpy as np
import traceback
import cv2
ocr_reader = None
class OCRWrapper:
    
    def __init__(self):
        # Initialize regex pattern and OCR reader
        self.ocr_reader = None
        self.pattern = re.compile(r'^[A-Z]\d{13}$')

        # Bitmasks for character validation
        self.uppercase_mask = 0
        for c in range(ord('A'), ord('Z') + 1):
            self.uppercase_mask |= 1 << c  # Set bit for A-Z
        self.digit_mask = 0
        for c in range(ord('0'), ord('9') + 1):
            self.digit_mask |= 1 << c  # Set bit for 0-9

        # Lookup tables for corrections (faster than dictionaries)
        self.first_char_corrections = [None] * 128  # ASCII range
        for src, dst in [('0', 'O'), ('1', 'I'), ('5', 'S'), ('8', 'B'), ('2', 'Z'), ('4', 'A'), ('6', 'G')]:
            self.first_char_corrections[ord(src)] = dst

        self.numeric_corrections = [None] * 128  # ASCII range
        for src, dst in [('O', '0'), ('o', '0'), ('I', '1'), ('l', '1'), ('i', '1'), ('Z', '2'), ('z', '2'),
                         ('S', '5'), ('s', '5'), ('B', '8'), ('A', '4'), ('a', '4'), ('G', '6'), ('g', '6'),
                         ('C', '0'), ('c', '0')]:
            self.numeric_corrections[ord(src)] = dst
    @staticmethod
    def initialize_ocr():
        global ocr_reader
        """Initialize the EasyOCR reader.

        Returns:
            bool: True if initialization succeeds, False otherwise.
        """
        try:
            ocr_reader = easyocr.Reader(['en'], gpu=False)
            print("EasyOCR initialized successfully.")
            return True
        except Exception as e:
            print(f"Failed to initialize EasyOCR: {e}")
            return False
    @staticmethod
    def correct_common_ocr_mistakes(text):
        corrections = {
            '1': 'T',  # ưu tiên sửa 1 thành T đầu tiên
            '0': 'O',
            '5': 'S',
            '8': 'B',
            '2': 'Z',
            '6': 'G'
        }
        # Sửa ký tự đầu tiên
        if len(text) > 0 and text[0] in corrections:
            text = corrections[text[0]] + text[1:]

        # Sửa các ký tự còn lại (số thành chữ)
       # text = ''.join(corrections.get(c, c) for c in text)

        return text
    def _correct_text( raw_text):
        """Correct OCR text using bitwise validation and lookup tables.

        Args:
            raw_text (str): Raw text from OCR.

        Returns:
            str or None: Corrected text if valid, None otherwise.
        """
        # Clean text in a single pass
        cleaned = [c for c in raw_text if c not in (' ', '-', '.', ',')]
        if len(cleaned) != 14:
            return None

        # Process first character
        first_char = cleaned[0]
        first_char_code = ord(first_char)
        corrected_first = None
        if self.uppercase_mask & (1 << first_char_code):  # Check if A-Z
            corrected_first = first_char
        elif self.first_char_corrections[first_char_code]:  # Check correction table
            corrected_first = self.first_char_corrections[first_char_code]
            print(f"Corrected first char: '{first_char}' -> '{corrected_first}'")
        else:
            return None

        # Process numeric part (characters 2-14)
        corrected_numeric = [''] * 13
        for i, char in enumerate(cleaned[1:]):
            char_code = ord(char)
            if self.digit_mask & (1 << char_code):  # Check if 0-9
                corrected_numeric[i] = char
            elif self.numeric_corrections[char_code]:  # Check correction table
                corrected_numeric[i] = self.numeric_corrections[char_code]
                print(f"Corrected numeric char: '{char}' -> '{corrected_numeric[i]}' in '{raw_text}'")
            else:
                return None

        # Combine and validate
        corrected_text = corrected_first + ''.join(corrected_numeric)
        if self.pattern.match(corrected_text):
            return corrected_text
        print(f"Warning: Corrected text '{corrected_text}' does not match regex.")
        return None
    @staticmethod
    def get_rotated_rect(bbox):
        pts = np.array(bbox, dtype=np.float32)
        rect = cv2.minAreaRect(pts)
        (cx, cy), (w, h), angle = rect

        if w < h:
            w, h = h, w
            angle = angle + 90

        return ((cx, cy), (w, h), angle)
    @staticmethod
    def rotate_image(image, angle):
        """Xoay ảnh theo góc angle (độ)."""
        (h, w) = image.shape[:2]
        center = (w // 2, h // 2)

        # Tính ma trận xoay
        M = cv2.getRotationMatrix2D(center, angle, 1.0)
        # Xoay ảnh
        rotated = cv2.warpAffine(image, M, (w, h), flags=cv2.INTER_LINEAR, borderMode=cv2.BORDER_REPLICATE)
        return rotated
    @staticmethod
    def find_ocr( roi_image):
        global ocr_reader
        texts=[]
        bboxs=[]
        confs=[]
        """Process an ROI image and return detected texts with confidence scores.

        Args:
            roi_image (np.ndarray): ROI image as a NumPy array (BGR format).

        Returns:
            list or None: List of (text, confidence) tuples, or None if an error occurs.
        """
        #roi_image=OCRWrapper.rotate_image(roi_image,-90)
        #cv2.imwrite("crop90.png",roi_image)
        # Validate input
        if not isinstance(roi_image, np.ndarray):
            print("Error: ROI image must be a NumPy array.")
            return bboxs,confs,texts
        if roi_image.size == 0 or roi_image.shape[0] == 0 or roi_image.shape[1] == 0:
            print("Error: Invalid or empty ROI image.")
            return bboxs,confs,texts
       
        if ocr_reader is None:
            print("Error: EasyOCR not initialized. Call initialize_ocr first.")
            return bboxs,confs,texts

        try:
            # Khởi tạo EasyOCR Reader
            #cv2.imwrite("rs.png",)
            gray = cv2.cvtColor(roi_image, cv2.COLOR_BGR2GRAY)
            #enhanced = cv2.equalizeHist(gray)
            #cv2.imwrite("rs.png",enhanced)
            #imgCheck = cv2.cvtColor(gray, cv2.COLOR_GRAY2BGR)
            # Nhận diện
                                
            #adjust_contrast=0.85,       # tăng contrast thu cong
            results = ocr_reader.readtext(
                    gray,
                    detail=1,
                    allowlist="ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                    paragraph=False,
                    text_threshold=0.3,        # thấp hơn để bắt cả text mờ,Bạn đang để thấp (0.4) → Dễ bắt chữ mờ hơn.
                    low_text=0.3,              # cho phép bắt các chữ rất mờ,Giá trị thấp → dễ bắt chữ hơn → có thể bắt nhầm noise.
                    link_threshold=0.9,       #: Ngưỡng để xác định ký tự nào nên ghép lại thành từ.
                    contrast_ths=0.5,            #contrat tu dong
                    mag_ratio=1.0,           #phong to
                    min_size=20,              #min size chu
                    rotation_info=[0],
                    decoder='greedy'  #greedy beamsearch nhanh hon
                    
                    
            )
            print(results)
            raw_texts = [(text, prob) for _, text, prob in results]
            # Vẽ box và score
            for (bbox, text, confidence) in results:
                rotated_rect = OCRWrapper.get_rotated_rect(bbox)
                bboxs.append(rotated_rect)
                confs.append(confidence)
                corrected_text = OCRWrapper.correct_common_ocr_mistakes(text)
                texts.append(corrected_text)
            return bboxs,confs,texts
            # Perform OCR
            """ results = self.ocr_reader.readtext(roi_image, detail=1, paragraph=False)
            print("Raw OCR results:")
            raw_texts = [(text, prob) for _, text, prob in results]
            for text, prob in raw_texts:
                print(f"- Text: '{text}', Confidence: {prob:.1f}")

            # Process and correct texts
            corrected_results = []
            for raw_text, prob in raw_texts:
                corrected_text = self._correct_text(raw_text)
                if corrected_text:
                    print(f"Validated: '{raw_text}' -> '{corrected_text}' (Confidence: {prob:.1f})")
                    corrected_results.append((corrected_text, prob))

            if not corrected_results:
                print("No valid texts found after correction and validation.")
            return corrected_results """

        except Exception as e:
            with open("error_log.txt", "a") as f:  # "a" = append, không ghi đè
             f.write("Error during OCR processing: {e}")
             traceback.print_exc(file=f)  # Ghi full stacktrace vào file
            print(f"Error during OCR processing: {e}")
            return None
"""import cv2
ocr = OCRWrapper()
if ocr.initialize_ocr():
    img = cv2.imread('demo1.png')
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    enhanced = cv2.equalizeHist(gray)
    image = cv2.cvtColor(enhanced, cv2.COLOR_GRAY2BGR)
    cv2.imwrite("rs.png",enhanced)
    ocr.find_ocr(image)
           
else:
        print("Failed to initialize EasyOCR.")"""
