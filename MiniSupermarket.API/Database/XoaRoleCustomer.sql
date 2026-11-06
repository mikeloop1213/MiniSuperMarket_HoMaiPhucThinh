USE ZenMart_HoMaiPhucThinh;
GO
BEGIN TRANSACTION;

-- xoa tai khoan mang role Customer (chua lap hoa don nao)
DELETE FROM Users
WHERE Role = 'Customer'
  AND UserId NOT IN (SELECT UserId FROM Orders);

-- neu con tai khoan Customer da co hoa don thi chuyen sang Cashier va khoa lai
UPDATE Users SET Role = 'Cashier', IsActive = 0 WHERE Role = 'Customer';

-- xoa role Customer khoi bang Roles
DELETE FROM Roles WHERE RoleName = 'Customer';

COMMIT TRANSACTION;
GO

SELECT * FROM Roles;
SELECT UserId, Username, FullName, Role, IsActive FROM Users;
