use QuanLySinhVien
go 
--create table LopSH(
--     Malop int primary key,
--	 Tenlop nvarchar (50),
--	 );
--CREATE TABLE SinhVien (
--    MaSV INT PRIMARY KEY ,
--    NameSV nvarchar(50),
--    Malop int,
--	NgaySinh date ,
--	DiemTB float,
--	Gender bit,
--	Anh bit,
--	CCCD bit,
--	HocBa bit,
--	foreign key (Malop) references LopSH(Malop)
--);
select * from LopSH
select * from SinhVien 
select * from SinhVien as S inner join LopSH as L on S.Malop = L.Malop

insert into LopSH values ('2111', '21TCLC-DT1');
insert into LopSH values ('2112', '21TCLC-DT2');
insert into LopSH values ('2113', '21TCLC-DT3');
insert into LopSH values ('2114', '21TCLC-DT4');




insert into  SinhVien values ('101', 'Nguyen Van A', '2112', '2003-04-12', '8.5', 1, 0, 1, 1);
insert into  SinhVien values ('102', 'Pham Hong Ngoc', '2111', '2003-06-24', '9.5', 0, 0, 1, 0);
insert into  SinhVien values ('103', 'Phan Thu Suong ', '2111', '2003-8-8', '7.5', 1, 1, 1, 1);
insert into  SinhVien values ('104', 'Dong Phuoc Cuong', '2113', '2003-04-3', '6.5', 1, 0, 0, 1);
insert into  SinhVien values ('105', 'Nguyen Thi Le', '2113', '2003-12-23', '9.5', 1, 0, 1, 0);

SELECT *
FROM SinhVien as  S inner JOIN  LopSH as L
ON L.Malop = S.Malop; 