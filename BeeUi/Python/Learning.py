import torch
import torchvision
import torchvision.transforms as T
import cv2
from ultralytics import YOLO
import time
import os
import numpy as np
#from MatCV import process_image  # Giả sử bạn đã tạo module này
# Đường dẫn đến mô hình YOLOv8 (.pt) và ảnh bạn muốn kiểm tra
model_path='NIDEC_5_5S.pt'
image_path='kd5.png'
models= {}
TypeModel=2
device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
class ObjectDetector:
        def __init__(self):
                self.models = {}
        @staticmethod
        def close():
                exit()
        @staticmethod
        def load_model(nameTool,model_path,Type):
                global models ,TypeModel
                TypeModel=Type
                    # Tải mô hình YOLOv8 từ tệp .pt
                if Type==1:
                        models[nameTool] = YOLO(model_path)
                elif Type==2:
                        # 🏷️ Định nghĩa mô hình Faster R-CNN
                        models[nameTool] = torchvision.models.detection.fasterrcnn_resnet50_fpn(pretrained=False)
                        num_classes = 6  # Số lớp (5 lớp + 1 lớp nền)
                        # 🏷️ Tạo đầu ra phù hợp số lớp
                        in_features = model.roi_heads.box_predictor.cls_score.in_features
                        models[nameTool].roi_heads.box_predictor = torchvision.models.detection.faster_rcnn.FastRCNNPredictor(in_features, num_classes)
                        # 🏷️ Tải trọng số từ tệp
                        
                        
                        if not os.path.exists(model_path):
                            print(f"❌ Không tìm thấy mô hình: {model_path}")
                            exit()
                        # 🔥 Tải trọng số vào mô hình
                        models[nameTool].load_state_dict(torch.load(model_path, map_location=device))
                        models[nameTool].to(device)
                        models[nameTool].eval() 
                return models
        @staticmethod
        def predict(image_array, confidence_threshold,nameTool):
                #image_array = cv2.imread(image_path)
                if isinstance(image_array, np.ndarray):
                    #cv2.imwrite("raw.png",image_array)
                    #input_tensor = torch.from_numpy(raw).permute(2, 0, 1).float() / 255.0
                        # Thêm một chiều batch
                    #input_tensor = input_tensor.unsqueeze(0)
                    global models,TypeModel,device  # Khai báo rằng chúng ta đang sử dụng biến toàn cục
                    model=models.get(nameTool)
                    if model is None:
                        raise ValueError("Mô hình chưa được tải. Vui lòng tải mô hình trước khi dự đoán.")
                    if TypeModel ==1:
                            results = model.predict(image_array, save=False, show=False, conf=confidence_threshold)
                            if isinstance(results, list) and len(results) > 0:
                                for result in results:
                                    #print(result)
                                    #print(results[0].boxes)
                                    
                                    if hasattr(result, 'boxes'):  # Kiểm tra xem đối tượng có thuộc tính 'boxes' không
                                        boxes = result.boxes  # Lấy các bounding boxes
                                        valid_boxes = []
                                        scores = []
                                        labels=[]
                                        # Nếu boxes là tensor
                                        for box in boxes.data:
                                            x1, y1, x2, y2, conf, cls = box.tolist()  # Chuyển đổi tensor thành danh sách
                                            class_id = int(cls)  # Đảm bảo ID lớp là số nguyên
                                            #if(model.names[class_id]!="dauloc"):
                                             #      continue
                                            valid_boxes.append((int(x1), int(y1), int(x2), int(y2)))
                                            scores.append(conf)
                                            labels.append(model.names[class_id])
                                        print("Bounding Boxes:", valid_boxes)
                                        print("Scores:", scores)
                                        return valid_boxes, scores,labels
                                    else:
                                        print("Đối tượng không có thuộc tính 'boxes'") 
                            else:
                                print("Không có bounding boxes được phát hiện.")
                                raise ValueError("Input must be a numpy array")
                    elif TypeModel ==2:
                            # 🔄 Chuyển ảnh về tensor cho mô hình
                            transform = T.Compose([T.ToTensor()])
                            image_tensor = transform(image_array).unsqueeze(0).to(device)
                            # 🕒 Đo thời gian bắt đầu
                            start_time = time.time()
                            # 🔍 Dự đoán bounding boxes
                            with torch.no_grad():
                                predictions = model(image_tensor)[0]
                            valid_boxes = []
                            scores = []
                            labels=[]
                            # 🎯 Lấy bounding boxes, labels và scores
                            valid_boxes = predictions["boxes"].cpu().numpy()
                            labels = predictions["labels"].cpu().numpy()
                            scores = predictions["scores"].cpu().numpy()
                            end_time = time.time()
                            cycle_time = end_time - start_time
                            print("Bounding Boxes:", valid_boxes)
                            print("Scores:", scores)
                            print("Label:", labels)
                            print(f"⏱️ Thời gian xử lý cho ảnh : {cycle_time:.4f} giây")
                            return valid_boxes,scores,labels
                else:
                    raise ValueError("Input must be a numpy array")
#ObjectDetector.load_model("HC",model_path,1)
#ObjectDetector.predict(0.5,"HC")
