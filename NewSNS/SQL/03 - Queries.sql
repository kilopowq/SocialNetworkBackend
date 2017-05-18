USE NewSNS;
GO

-- ** Total list of friends
--
select fr.FirstName + ' ' + fr.LastName as userFullName,
		u2.FirstName + ' ' + u2.LastName as friendFullName
	from
		(select f.FriendID, u.FirstName, u.LastName from tblFriend f
			inner join tblUser u
				on f.UserID = u.Id) fr
	inner join tblUser u2
		on u2.Id = fr.FriendID


-- ** Friends of some person
--
--select friends.userFullName, friends.friendFullName
--	from
--		(select fr.FirstName + ' ' + fr.LastName as userFullName,
--				u2.FirstName + ' ' + u2.LastName as friendFullName
--			from
--				(select f.FriendID, u.FirstName, u.LastName from tblFriend f
--					inner join tblUser u
--						on f.UserID = u.Id) fr
--				inner join tblUser u2
--					on u2.Id = fr.FriendID) friends
--	where friendFullName = 'Roman Tarasiuk'