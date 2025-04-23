===================================== CẤU TRÚC DỰ ÁN ASP.NET API =====================================

docker-compose.yml để định nghĩa và chạy nhiều container Docker cùng một lúc: 1 container rabbitmq, 3 container daatabase, 4 container service

4 service:
	1. OrderService.WebAPI: chứa controller và config database OrderDbContext, dockerfile cho order-service
	2. PaymentService.WebAPI: chứa controller và config database PaymentDbContext, dockerfile cho payment-service
	3. ReportService.WebAPI: chứa controller và config database ReportDbContext, dockerfile cho report-service
	4. ApiGatewayOcelot: cấu hình port map đến các port của các service khác, dockerfile cho api-gateway

Chi tiết trong một service:
		- Program.cs là nơi khởi đầu cho service:
			-- Khởi tạo builder để cấu hình các dịch vụ (services, middleware, DI, ...)
			-- Đăng ký các services vào Dependency Injection container
			-- Cấu hình các middleware trong pipeline (routing, auth, CORS, ...)
		- appsettings.json là một Configuration file để lưu trữ các thông tin cấu hình cho service
		- launchSettings.json là một Configuration for Development để cấu hình các profile và biến môi trường khi khởi động ứng dụng trong môi trường phát triển
		- Models định nghĩa các Entity của database 
		- Data để tạo database kế thừa DbContext của Entity Framework Core, sử dụng migration để tạo database tương tự trên postgreSQL
		- DTOs để tạo các dto cho req và res giữa client và server
		- Repository:
			-- Interface để định nghĩa những chức năng sẽ triển khai 
			-- Class kế thừa interface đó để tạo các chức năng với mục đích là thao tác với database
		- Controllers chứa api controller là nơi tiếp nhận request từ client, inject repository và trả về response cho client
		- Events:
			-- Models định nghĩa các dữ liệu nào sẽ được gửi đi và nhận về cho các message
			-- Interface để định nghĩa các phương thức liên quan đến việc gửi message đến RabbitMQ
			-- RabbitMQ là class kế thừa từ interface trên để tạo các phương thức gửi message
		- BackgroundServices:
			-- Một abstract class được kế thừa từ BackgroundService có chức năng lắng nghe các thông điệp từ RabbitMQ, xử lý các thông điệp và xác nhận việc tiêu thụ thông điệp sau khi xử lý
			-- Các class kế thừa từ abstract class trên thực hiện lắng nghe và xử lý sự kiện cụ thể từ RabbitMQ
		- Services là nơi chứa các Processor chịu trách nhiệm xử lý các nghiệp vụ chính và tạo/gửi các event cần thiết
		- Dockerfile để xây dựng và chạy service trong môi trường Docker

Flow-Api:
	Client gửi request lên port 8000 của ApiGatewayOcelot
	Ocelot điều hướng tới port của service tương ứng
	Gọi tới api trong controller (nhận request) --> Repository (connect database) --> Data --> response
	Trả về response cho client

Flow đặt hàng:
	Client gửi request đặt hàng --> ocelot điều hướng tới router tương ứng
	Gọi api tạo đơn hàng trong controller --> repository --> data --> OK
	Tạo event đặt hàng trong processor: order_exchange với routingKey là order.created
	Lắng nghe order_exchange (order.created) với queue order_created_test_queue --> thực hiện thêm thanh toán trong PaymentService (được định nghĩa trong PaymentProcessor)
	Lắng nghe order_exchange (order.created) với queue order_created_report_test_queue --> thực hiện thêm đơn hàng trong ReportService (được định nghĩa trong ReportProcessor)
	Thanh toán thành công gửi event thành công lên payment_exchange với routingKey là payment.succeeded; nếu thất bại thì routingKey là payment.failed
	Lắng nghe payment_exchange (payment.succeeded) với queue payment_succeeded_test_queue --> thực hiện update trạng thái đơn hàng trong OrderService (được định nghĩa trong OrderProcessor)
	Lắng nghe payment_exchange (payment.succeeded) với queue payment_succeeded_report_test_queue --> thực hiện thêm thanh toán thành công trong ReportService (được định nghĩa trong ReportProcessor)
	Lắng nghe payment_exchange (payment.failed) với queue payment_failed_test_queue --> thực hiện update trạng thái đơn hàng trong OrderService (được định nghĩa trong OrderProcessor)

Tổng kết có 2 Exchange:
	- order_exchange:
		-- routingKey: order.created 
			--- queue: order_created_test_queue ===> lắng nghe sự kiện tạo order thành công để thanh toán
			--- queue: order_created_report_test_queue ===> lắng nghe sự kiện tạo order thành công để thêm dữ liệu đơn đặt hàng vào report database
	- payment_exchange:
		-- routingKey: payment.succeeded 
			--- queue: payment_succeeded_test_queue ===> lắng nghe sự kiện thanh toán thành công để update trạng thái thanh toán cho đơn hàng
			--- queue: payment_succeeded_report_test_queue ===> lắng nghe sự kiện thanh toán thành công để thêm dữ liệu thanh toán vào report database
		-- routingKey: payment.failed 
			--- queue: payment_failed_test_queue ===> lắng nghe sự kiện thanh toán thất bại để update trạng thái thanh toán cho đơn hàng


Phần giả lập thanh toán em tạm hardcode mã PIN là 1111, mục đích để khi client nhập sai mã PIN sẽ tạo sự kiện thanh toán thất bại với status là Failed.