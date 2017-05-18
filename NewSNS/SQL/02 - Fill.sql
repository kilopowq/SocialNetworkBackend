USE NewSNS;
GO

DELETE FROM tblFriend
GO

DELETE FROM tblUser
GO

DELETE FROM tblUserState
GO


--
--
--
SET IDENTITY_INSERT tblUserState ON

INSERT INTO tblUserState([Id], [Name])
	VALUES
		(1, 'New'),
		(2, 'Active'),
		(3, 'Blocked'),
		(4, 'Closed')

SET IDENTITY_INSERT tblUserState OFF


--
--
--
SET IDENTITY_INSERT tblUser ON

INSERT INTO tblUser (
		[Id],
		[StateId],
		[FirstName],
		[LastName],
		[Login],
		[Password],
		[BirthDate],
		[Email],
		[Phone]
	)
	VALUES
		(0
			,2
			,'sa'
			,'System Administrator'
			,'sa'
			,'AwD5PZyXQqZ8cZjZ0oXIgZIBJYmh2jJZvMvFL5I92rMqAqPE0JiXx9ds+sg7QlP1Cq1vuT24KReuM/aQUszv0/jvWIQ='
			,'1979-10-28'
			,'roman.tarasiuk.l@gmail.com'
			,'+380675825166'),
		(1
			,4
			,'Roman'
			,'Tarasiuk'
			,'TRM'
			,'123'
			,'1979-10-28'
			,'roman.tarasiuk.l@gmail.com'
			,'+380675825166'),
		(2
			,2
			,'Ірина'
			,'Тарасюк'
			,'Iryna'
			,'wLR/I+5M8LDp8Xg67yk/LdbXKFSkIrhmPFqnqLa5H6PzDNtwADxQtIVy1HVIj5mTiztHk3XwPEkj/oKyabXeuMsxPLM='
			,'2016-05-09'
			,'iryna.tarasiuk@gmail.com'
			,'+380675825166'),
		(3
			,2
			,'Наталія'
			,'Тарасюк'
			,'Nata'
			,'555'
			,'1981-09-09'
			,'nanaliya.tarasiuk@gmail.com'
			,'+380675825166'),
		(4
			,2
			,'Василь'
			,'Тарасюк'
			,'Vasko'
			,'777'
			,'1984-01-11'
			,NULL
			,NULL),
		(5
			,2
			,'Ярослав'
			,'Баймак'
			,'Slavko'
			,'999'
			,'1986-01-10'
			,'NULL'
			,'NULL'),
		(6
			,2
			,'Микола'
			,'Гривнак'
			,'Kolka'
			,'111'
			,'1970-04-16'
			,'NULL'
			,'NULL')

SET IDENTITY_INSERT tblUser OFF
GO


--
--
--
INSERT INTO tblFriend ([UserID], [FriendID])
	VALUES
		(1, 2),
		(1, 3),
		(1, 4),
		(1, 5),
		(1, 6),
		(2, 1),
		(2, 3),
		(2, 4),
		(3, 1),
		(3, 2),
		(3, 5),
		(4, 1),
		(4, 2),
		(4, 6),
		(5, 1),
		(5, 3),
		(6, 1),
		(6, 4)
GO
