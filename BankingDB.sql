create table USERS
(
	userNumber int identity(10000,1) primary key,
	accName nvarchar(40) not null,
	accUsername nvarchar(20) unique not null,
	accPassword nvarchar(20) not null,
	accSSN nvarchar(20) unique not null,
	accEmail nvarchar(50) unique not null,
	accPhone nvarchar(20) not null,
	isAdmin bit not null default 0
)

create table ACCOUNTS
(
	accNumber int identity(10000,1) primary key,
	userNumber int not null,
	accType nvarchar(10),
	accBalance decimal(10,2) default 0.00,
	foreign key (userNumber) references USERS(userNumber)
)

create table TRANSACTIONS
(
	transNumber int identity(10000,1) primary key,
	accNumber int not null,
	userNumber int not null,
	transAmount decimal(10,2) not null,
	transDescription nvarchar(100),
	transDate date not null default convert(date, getdate()),
	transDateTime datetime not null default getdate(),
	foreign key (accNumber) references ACCOUNTS(accNumber),
	foreign key (userNumber) references USERS(userNumber)
)

insert into USERS values('Jacob Wolfe', 'admin', 'Admin@1234', '764-32-1587', 'admin@banking.com', '515-321-6548', 1)

select * from USERS

insert into USERS (accName, accUsername, accPassword, accSSN, accEmail, accPhone) values ('Fanchon Stoakley', 'fstoakley0', 'mHuGb7aAG37', '434-98-9625', 'fstoakley0@skype.com', '483-999-9869');

select top 100 * from USERS

insert into ACCOUNTS values()

select * from USERS where isAdmin=1

select * from ACCOUNTS

insert into ACCOUNTS (userNumber, accType) values(10000, 'Checkings')

delete from ACCOUNTS

drop table TRANSACTIONS, ACCOUNTS

select top 100 * from ACCOUNTS

insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (10000, 10000, 500.22, 'Initial Deposit')

select * from TRANSACTIONS

select count(*) from ACCOUNTS

select count(*) from USERS where accUsername='admin' and accPassword='Admin@1234' and isAdmin=1

select userNumber from USERS where accUsername='admin' and accPassword='Admin@1234'

select top 10 * from TRANSACTIONS order by userNumber desc

delete from USERS where userNumber=11002

select userNumber, accBalance from ACCOUNTS where accNumber=10000

select sum(accBalance) from ACCOUNTS

select count(*) from ACCOUNTS where accType='Checkings'