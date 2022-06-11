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
	[Id] blob primary key not null,
	[Name] text,
	[IpAddress] blob,
	[MacAddress] blob
);

create table [Server] (
	[Id] blob primary key not null,
	[Name] text,
	[ComputerId] blob not null references [Computer] ([Id])
);

insert into [User] ([Login], [PasswordHash])
values
	('Admin', X'c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f');
	
insert into [Computer] ([Id], [Name], [IpAddress], [MacAddress])
values
	(X'3c4989e7fbb7894e941789f0b97faac1', 'MainComputer', null, null);
	
insert into [Server] ([Id], [Name], [ComputerId])
values
	(X'3a1364a9a9d19d46bdef768805612025', '1.12.2 Server', X'3c4989e7fbb7894e941789f0b97faac1')