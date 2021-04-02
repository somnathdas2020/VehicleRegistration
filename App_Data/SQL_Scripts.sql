select * from sys.tables
select * From AspNetUsers

Create table tblCustomerDetails
(
	CustomerID [nvarchar](128) not null primary key,
	CustomerName nvarchar(100) not null
)

Create Table tblVehicle
(
	[VehicleID] int not null identity(1,1) primary key ,
	[VehicleName] NVARCHAR(200) not null
)

Create Table tblVehicleRegistrationDetails
(
	VRegistrationID int not null identity(1,1) primary key ,
	VRegistrationAmount [decimal](13,2) not null,
	VRegistrationYear [int] not null,
	AllRegAmountPaid int default 0,
	VehicleID int not null,
	CustomerID [nvarchar](128) not null,
)
Create table tblCustomerRegistrationPaymentDetail
(
	RegistrationID int not null,
	PaidAmount decimal(13,2) not null,
	DueAmount decimal(13,2) not null,
)

ALTER View VW_VehicleRegistrationDetails
as
	SELECT VRegistrationID as RegistrationID, CustomerName, VehicleName, VRegistrationAmount as RegistrationAmount, VRegistrationYear as RegistrationYear, 
	AllRegAmountPaid, ISNULL(PaidAmount,0) AS PaidAmount, ISNULL(DueAmount,VRegistrationAmount) AS DueAmount 
	FROM AspNetUsers AS ANU
	INNER JOIN tblCustomerDetails AS CD ON ANU.Id = CD.CustomerID
	INNER JOIN tblVehicleRegistrationDetails AS VR ON CD.CustomerID = VR.CustomerID
	INNER JOIN tblVehicle AS V ON VR.VehicleID = V.VehicleID
	LEFT OUTER JOIN tblCustomerRegistrationPaymentDetail AS RAPAID ON VR.VRegistrationID = RAPAID.RegistrationID

SELECT * FROM VW_VehicleRegistrationDetails

INSERT INTO tblCustomerDetails(CustomerID, CustomerName) 
VALUES
('8d67a39c-d9e5-421e-bb3d-f7e183d47638', 'Somnath Das'),
('714f317a-639f-4c0e-9d70-051fdcfc46bf', 'William'),
('1b1f499f-1ac2-4618-82b9-41bb89afee47', 'James')

insert into tblVehicle(VehicleName)
values('Car'),('Bike')

insert into tblVehicleRegistrationDetails(VRegistrationAmount, VRegistrationYear, AllRegAmountPaid, VehicleID, CustomerID)
values  (100, 2021, 0, 1, '714f317a-639f-4c0e-9d70-051fdcfc46bf'),
		(50, 2021, 1, 2, '1b1f499f-1ac2-4618-82b9-41bb89afee47')

INSERT  INTO tblCustomerRegistrationPaymentDetail (RegistrationID, PaidAmount, DueAmount)
VALUES (1, 40, 60)

alter procedure getVehicleRegistrationDetails
as
begin
	SELECT RegistrationID, CustomerName, VehicleName, RegistrationAmount, RegistrationYear, AllRegAmountPaid, PaidAmount, DueAmount  
	FROM VW_VehicleRegistrationDetails
end

alter procedure getVehicleRegistrationDetail
(
	@RegistrationID int
)
as
begin
	SELECT CustomerName, RegistrationAmount, AllRegAmountPaid, PaidAmount, DueAmount  
	FROM VW_VehicleRegistrationDetails where RegistrationID = 1 
end


ALTER procedure payRegistrationAmount
(
	@RegistrationID int, 
	@PaymentAmount decimal(13,2)
)
as
begin
	declare @dueAmount decimal(13, 2), @status int
	set @dueAmount = (select IsNull(DueAmount, RegistrationAmount) - @PaymentAmount from VW_VehicleRegistrationDetails where RegistrationID = @RegistrationID)
	set @dueAmount = (select case when @dueAmount < 0 then 0 else @dueAmount end)
	if exists (select 1 from tblCustomerRegistrationPaymentDetail where RegistrationID = @RegistrationID)
	begin
		UPDATE tblCustomerRegistrationPaymentDetail SET PaidAmount = PaidAmount + @PaymentAmount, DueAmount = @dueAmount WHERE RegistrationID = @RegistrationID
	end
	else
	Begin
		insert into tblCustomerRegistrationPaymentDetail(RegistrationID, PaidAmount, DueAmount)
		select @RegistrationID, @PaymentAmount, @dueAmount from tblVehicleRegistrationDetails where VRegistrationID = @RegistrationID	

	end
	set @status = @@ROWCOUNT 

	update tblVehicleRegistrationDetails Set AllRegAmountPaid = case when @dueAmount > 0 then 0 else 1 end where VRegistrationID = @RegistrationID
	select @status as NoOfRowExecuted
end
