create table [User] (
	[Login] text primary key not null,
	[PasswordHash] blob not null
);

create table [Session] (
	[Id] blob primary key not null,
	[UserLogin] text not null references [User] ([Login]),
	[ExpiresAt] integer default (strftime ('%ms', 'now')) not null
);

create table [Computer] (
	[Id] integer primary key autoincrement not null,
	[Name] text,
	[IpAddress] blob,
	[MacAddress] blob
);

create table [Server] (
	[Id] integer primary key autoincrement not null,
	[Name] text,
	[ComputerId] int not null references [Computer] ([Id])
);

insert into [User] ([Login], [PasswordHash])
values
	('Admin', X'c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f');
	
insert into [Computer] ([Name], [IpAddress], [MacAddress])
values
	('MainComputer', null, null);
	
insert into [Server] ([Name], [ComputerId])
values
	('1.12.2 Server', 1)