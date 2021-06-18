CREATE TABLE [dbo].[payment]
(
	[Id] INT PRIMARY KEY REFERENCES workers(Id),
	[month] VARCHAR(32),
	[amount_in_PLN] FLOAT, 
)
