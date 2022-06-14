create table [User] (
	[Login] text primary key not null,
	[PasswordHash] blob not null
);

create table [VkUser] (
	[Id] integer primary key not null,
	[UserLogin] text not null references [User] ([Login])
);

create table [Session] (
	[Id] blob primary key not null,
	[UserLogin] text not null references [User] ([Login]),
	[ExpiresAt] integer not null
);

create table [Computer] (
	[Id] blob primary key not null,
	[Name] text,
	[Alias] text unique not null,
	[IpAddress] blob,
	[MacAddress] blob
);

create table [Server] (
	[Id] blob primary key not null,
	[Name] text,
	[Alias] text unique not null,
	[ComputerId] blob not null references [Computer] ([Id])
);

insert into [User] ([Login], [PasswordHash])
values
	('Admin', X'c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f');
	
insert into [Computer] ([Id], [Name], [Alias], [IpAddress], [MacAddress])
values
	(X'3c4989e7fbb7894e941789f0b97faac1', 'MainComputer', 'main', null, null);
	
insert into [Server] ([Id], [Name], [Alias], [ComputerId])
values
	(X'3a1364a9a9d19d46bdef768805612025', '1.12.2 Server', 'server', X'3c4989e7fbb7894e941789f0b97faac1')