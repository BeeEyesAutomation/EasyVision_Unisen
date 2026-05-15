# Plan - Khac phuc Tool Pattern bat sai mau va toi uu `Pattern2.cpp`

## Muc tieu

Tool Pattern dang co truong hop bat nham vung nen/canh cheo thay vi mau that. Trong anh loi, vung bi khoanh do co canh/duong cheo du tuong quan de sinh candidate, trong khi hinh mau that la khoi trang co bien va hinh dang ro hon. Muc tieu la giam false positive ma khong lam mat true positive, sau do moi toi uu toc do.

## Hien trang code

- `Pattern/Pattern2.cpp` co `Match()` co dien: quet goc theo pyramid, lay cuc dai NCC, refine xuong layer thap, loc score va NMS rotated-rect.
- `MatchStable()` da co pipeline tot hon: preprocess, scale bank, coarse `Match()`, validator `raw/grad/edgeIoU/edgeRatio`, final score, keep filter, NMS.
- `Pattern/Pattern2.h` da co `Pattern2StableConfig`, `Pattern2PreprocessConfig`, preset preprocess, debug log, scale search, GPU, CPU multi-thread.
- Phien truoc da them `RansacLineCore`/`RansacLineCli`; co the tan dung de kiem tra cap canh song song/centerline cua mau neu hinh mau co cau truc canh on dinh.

## Nguyen nhan kha di

1. `RelaxedRawScore` trong coarse stage thap, cho phep nhieu candidate nen di vao validator.
2. NCC tho co the cao voi mang canh cheo/o chu nhat vi template sau preprocess con tuong quan voi vung nen.
3. `edgeRatio` chi so so luong edge, chua du de phan biet topology canh cua mau that voi canh nen.
4. `edgeIoU` co the mem qua voi template it chi tiet hoac anh sang khong deu.
5. NMS xu ly chong lap sau cung, nhung khong hieu candidate nao dung ve hinh hoc.

## Chien luoc sua

### 1. Tao baseline co the lap lai

Luu debug log cho anh OK va anh bat sai:

- `coarseCount`
- tung candidate: center, angle, scale, coarse, raw, rawEdge, grad, edgeIoU, edgeRatio, final, keep reason
- anh preprocess preview: gray, edge magnitude, edge binary

Chi chinh thuat toan sau khi thay candidate dung va sai khac nhau o metric nao.

### 2. Tang suc phan biet shape validator

Uu tien sua trong `MatchStable()`:

- Them metric `edgeCentroidDistance`: khoang cach tam edge ROI voi tam edge template.
- Them metric `edgeProjectionSimilarity`: so histogram chieu X/Y hoac theo truc xoay cua template.
- Them metric `contourShapeRatio`: so contour, dien tich contour lon nhat, bounding rect/aspect cua edge.
- Voi mau co 2 canh song song ro, goi RANSAC line validator de kiem tra huong canh va khoang cach centerline.
- Candidate chi duoc giu khi qua `finalScore` va it nhat mot nhom hinh hoc manh, khong chi `coarseScore`.

### 3. Tuning threshold thich nghi

Dieu chinh `BuildAutoGate()` va `ApplyDifficultyToScaleTemplate()` theo thong ke template:

- Template edge-sparse hoac entropy thap: tang yeu cau `hardEdgeIou`, `hardEdgeRatio`, giam quyen cuu `keepRescue`.
- Template co edge density tot: dung `GrayPlusEdge` va tang trong so edge/grad.
- Khi nguoi dung set `MinAcceptScore`, van giu score nhung khong bo qua shape gate.

### 4. Kiem soat candidate va NMS

- Gioi han candidate tu coarse theo block/local max de giam nhieu nen.
- Reject som candidate ROI invalid, sat bien, kich thuoc lech template, hoac angle lech ngoai range sau refine.
- Sau validator moi NMS, sort theo `finalScore`, neu dong diem uu tien `edgeIoU`/shape metric tot hon.

### 5. Toi uu sau correctness

Chi toi uu sau khi false-positive case da bi loai:

- Cache `srcGrayPre`, `srcEdgeMag`, `srcEdgeBin`, gradient va edge masks theo scale.
- Reuse `roiPad`, `roiGray`, `roiNorm`, `roiGrad`, `roiEdge` trong validator loop.
- Khong copy/clone neu co the dung ROI view an toan.
- Do rieng CPU single-thread, CPU multi-thread, va GPU/OpenCL vi GPU co the cham hon voi ROI nho.

## Thu tu trien khai de xuat

1. `P2-001`: tao baseline debug/replay.
2. `P2-002`: them shape validator va reject false positive.
3. `P2-003`: tune auto threshold theo template stats.
4. `P2-004`: on dinh candidate/NMS.
5. `P2-005`: toi uu toc do, co benchmark truoc/sau.

## Tieu chi hoan thanh

- Anh OK van tra dung object voi score on dinh.
- Anh loi khong con nhan vung khoanh do la match hop le.
- Debug log giai thich ro ly do reject candidate sai.
- Release x64 build pass.
- Khong doi API C# <-> C++/CLI neu chua co ke hoach migration rieng.

