USE ZenMart_HoMaiPhucThinh;
GO
BEGIN TRANSACTION;

-- doi vai tro cua nhan vien hien co
UPDATE Users SET Role = 'Cashier'   WHERE Role = N'Thu ngân';
UPDATE Users SET Role = 'Warehouse' WHERE Role = N'Quản lý kho';
UPDATE Users SET Role = 'Admin'     WHERE Role = N'Quản lý cửa hàng';

-- bang Roles: Admin, Cashier, Warehouse
UPDATE Roles SET RoleName = 'Cashier',   Description = N'Nhân viên thu ngân, chỉ bán hàng tại quầy POS' WHERE RoleName = N'Thu ngân';
UPDATE Roles SET RoleName = 'Warehouse', Description = N'Quản lý nhóm hàng, sản phẩm và tồn kho'       WHERE RoleName = N'Quản lý kho';
DELETE FROM Roles WHERE RoleName = N'Quản lý cửa hàng';

COMMIT TRANSACTION;
GO

SELECT * FROM Roles;
SELECT UserId, Username, FullName, Role, IsActive FROM Users;
