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
    
create table shift(
	shift_id int auto_increment primary key,
    user_id int,
    time_start time,
    time_end time,
    constraint fk_shift_guards foreign key(user_id) references guards(guard_id)
);
select * from shift;

create table PriceTable(
	price_id int auto_increment primary key,
    time_min time,
    time_max time,
    price int not null,
    decription nvarchar(50)
);

select price from PriceTable where price_id = '1';

create table RegularCustomers(
	customer_id int primary key,
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

insert into RegularCustomers(customer_id, regular_name) values
(001, 'hhhh');

insert into ParkingCards(card_id, vehicle_number, vehicle_type, color, customer_id, descrip, expiry_date) values
(00001, '37A178', 'Way', 'Red', 001, null, '2008-11-11'),
(00002, '37B178', 'Way', 'Blue', 001, null, '2021-11-11'),
(00003, '37C178', 'Way', 'Blue', null, 'dayCard', '2008-11-11'),
(00004, '37D178', 'Way', 'Blue', null, 'dayCard', '2008-11-11'),
(00005, '37E178', 'Way', 'Blue', null, 'dayCard', '2008-11-11'),
(00006, '37F178', 'Way', 'Blue', null, 'dayCard', '2008-11-11');

insert into CheckInOut(card_id, checkin_time, checkin_user_id, vehicle_number, checkout_time, total) values 
('005','2021-10-06 19:25:00','2','37e178', null, null),
('004','2021-10-07 14:05:00','2','37d178', current_time(), 50000),
('003','2021-10-06 15:45:00','1','37b178', null, null),
('006','2021-10-07 14:25:00','1','37f178', null, null);

insert into PriceTable(time_min, time_max, price, decription) values 
('06:00','17:59', '25000', 'Per hour'),
('18:00','22:59', '35000', 'Per hour'),
('23:00','05:59', '100000', 'morning x 4');