## git 
- luôn get code mới từ main
- luôn checkout từ main khi làm tính năng mới
- merge code từ main liên tục để hạn chế conflict
----------------
3 lớp 
API 
BAL
DAL
## coding 
- luôn luôn đọc lỗi khi có exception, (message + innerException)
## DAL
- sử dụng repository - unitofwork
- sử dụng EF - code first 
## Swagger
- viết doc cho từng API,
- phải hiểu được API đó làm gì 
- phải hiểu được từng property của model là gì nếu có 
## report 
- ghi rõ chi tiết ai làm API nào
## abc
- chiều nay (17h) (10/10/2022) chốt là có làm repository, unitofwork hay không - có làm
- sáng mai phải hoàn thành được entities + code first 
- chiều mai phải hoàn thành được danh sách API 
## note 
-- 12/10
- public string? ClassCode { get; set; }
- entity tự làm không export từ db ra
- merge code lên main trước mỗi buổi review
- chốt doc mới 0h ngày 13/10
- entity 0h ngày 14/10
- API + doc 14h ngày 14/10

-- 14/10
- đặt lại cái route cho phù hợp  ```/api/Class/GetClass/{classId}``` ```api/Class/{classId}/GetAttendees``` thảo luận đưa ra deadline mới trong hôm nay -> chốt 0h ngày 19/10
- sửa lại dbcontext
- về tìm hiểu "OOP" "3 layers" "c# dependency injection" "async await" "dynamic order in linq" - thứ 4 19/10 sẽ hỏi random 1 số bạn 

-- 19/10
```
    API: ~70
    người: ~30
    total : 140h 
    => 1 người cần ~5h là xong => làm tròn lên 12h - mỗi người mỗi API khác nhau 
    => 1 người cần 36h để code xong 2 cái API 
 ```
 - chốt deadline làm xong logic cho API vào 11h59 tối nay 19/10
 - gắn unitofwork + repository vô project 
 - automapper

-- 26/10
- fluentvalidation
- try catch -> global exception handler   
- Unit test -> 
- không lưu từng entity 1, mà lưu theo batch mối quan hệ của nó
- sửa lại baserepository để mặc định là luôn luôn exclude entities đã được xóa
- thống nhất sử dụng repostiroy + unitofwork theo 1 cách duy nhất cho cả lớp
- sáng t6 ngày 28/10, lớp phải review với nhau trước - xem có những lỗi cơ bản hay không

-- 28/10 
- lưu data 1 lần duy nhất 
- get data sử dụng Include và ThenInclude
- sửa lại document của swagger
- api chạy được, logic khoản 7-80%  -> logic = 90%
- sáng thứ 2 chốt deadline 

-- 02/11 
- cuối ngày thứ 3 tuần sau, phải fix hết mấy cái note trên excel
- nhóm làm deployment phải có account trước ngày 03/11 - nhóm 1
- xóa bớt API dư
- **phân trang - sort - search - get all - chung 1 api -> phải giống kiểu dữ liệu trả về**

-- 09/11 
- **phân trang - sort - search - get all - chung 1 api -> phải giống kiểu dữ liệu trả về** -> xử lý ở database sử dụng linq + EF
- add author - PermissionAuthorize
- report
- 09/11 -> 16/11 bạn nào có mong muốn làm FE thì tạo thành 1 nhóm, **nếu** đủ từ 5->10 bạn trở lên thì lớp sẽ chia làm 2 team, 1 team BE + 1 team FE , **nếu** không đủ thì vẫn giữ nguyên
- các bạn làm BE có nhiệm vụ sửa lại documents (use case, activity diagram, class diagram)

-- 23/11 
- Lớp tập trung sửa lại document
- sau đó làm UT 
- sẽ có requirement mới trong tuần này để bổ sung
