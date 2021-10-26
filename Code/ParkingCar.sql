drop database if exists CarParkingDB;

create database CarParkingDB;

use CarParkingDB;

create table Guards(
	guard_id int auto_increment primary key,
    guard_account varchar(100) not null unique,
    guard_pass varchar(200) not null,
    guard_name varchar(100) not null,
    guard_address varchar(200)
);

create user if not exists 'vtca'@'localhost' identified by 'vtcacademy';
grant all on CarParkingDB.* to 'vtca'@'localhost';

insert into Guards(guard_account, guard_pass, guard_name) values
	('guard01', 'da8b32116716ab47668d4e55a0f26165' , 'guard'),
    ('guard02', 'f72d9ecfa2428b7fd219cb7fe6427737' , 'guard');

create table PriceTable(
	price_id int auto_increment primary key,
    time_min time,
    time_max time,
    price int not null,
    decription nvarchar(50)
);

create table RegularCustomers(
	customer_id int auto_increment primary key ,
    regular_name nvarchar(50) not null,
    email nvarchar(50),
    address nvarchar(100),
    phone_number int
);

create table ParkingCards(
	card_id int auto_increment primary key,
    vehicle_number varchar(15),
    vehicle_type nvarchar(50),
    color nvarchar(15),
    descrip nvarchar(255),
    customer_id int,
    expiry_date date not null,
    constraint fk_ParkingCard_Customer foreign key(customer_id) references RegularCustomers(customer_id)
);

create table CheckInOut(
	inout_id int auto_increment primary key,
    card_id int,
    checkin_time datetime,
    checkin_user_id int,
    checkout_time datetime,
    checkout_user_id int,
    vehicle_number varchar(15) not null,
    quantity int,
    price_id int,
    total int,
    constraint fk_CheckInOut_ParkingCard foreign key(card_id) references ParkingCards(card_id),
    constraint fk_CheckInOut_PriceTable foreign key(price_id) references PriceTable(price_id),
    constraint fk_CheckIn_Guard foreign key(checkin_user_id) references Guards(guard_id),
    constraint fk_CheckOut_Guard foreign key(checkout_user_id) references Guards(guard_id)
);

insert into RegularCustomers(regular_name, email, phone_number) values
('Nguyen Van A', 'nvA@gmail.com', '0123456789'),
('Nguyen Van B', 'nvB@gmail.com', '0987654321'),
('Nguyen Thi C', 'ntC@gmail.com', '0159786324');

insert into ParkingCards(vehicle_number, vehicle_type, color, customer_id, descrip, expiry_date) values
('37A178', 'Way', 'Red', 001, null, '2008-11-11'),
('37B178', 'Way', 'Blue', 002, null, '2021-11-11'),
('37C178', 'Way', 'Blue', null, 'dayCard', '2008-11-11'),
('37D178', 'Way', 'Blue', null, 'dayCard', '2008-11-11'),
('37E178', 'Way', 'Blue', null, 'dayCard', '2008-11-11'),
('37F178', 'Way', 'Blue', 003, null, '2021-11-11'),
('37g178', 'Way', 'Blue', null, 'dayCard', '2008-11-11');

insert into CheckInOut(card_id, checkin_time, checkin_user_id, vehicle_number, checkout_time, total) values 
('002','2021-10-07 14:05:00','2','37d178', null, null),
('003','2021-10-15 22:45:00','1','37b178', null, null),
('004','2021-10-15 17:25:00','1','37f178', null, null);

insert into PriceTable(time_min, time_max, price, decription) values 
('06:00','17:59', '25000', 'Per hour'),
('18:00','22:59', '35000', 'Per hour'),
('23:00','05:59', '100000', 'morning x 4'),
('00:00','23:59', '200000', 'Full day');

select * from checkinout;
select datediff(checkout_time, checkin_time) as date from CheckInOut where inout_id = '3';
select datediff(ADDTIME(checkout_time, '01:00'), ADDTIME(checkin_time, '01:00')) from CheckInOut where inout_id = '3';
select hour(timediff( '23:00', time (checkin_time))) as hour from CheckInOut where inout_id = '3';