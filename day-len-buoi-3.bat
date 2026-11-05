@echo off
chcp 65001 >nul
cd /d "%~dp0"
where git >nul 2>nul || set "PATH=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd;C:\Program Files\Git\cmd;%PATH%"
echo === Lay du lieu moi tu GitHub ===
git fetch origin || goto :err
echo === Chuyen sang nhanh buoi-5 (giu nguyen file cua ban) ===
git branch -f buoi-5 origin/buoi-5 || goto :err
git symbolic-ref HEAD refs/heads/buoi-5 || goto :err
git branch --set-upstream-to=origin/buoi-5 buoi-5
git reset -q
echo === Commit ===
git add -A || goto :err
git -c user.name="Ho Mai Phuc Thinh" -c user.email="hophucthinh876@gmail.com" commit -m "Buoi 5" || goto :err
echo === Push len GitHub ===
git push origin buoi-5 || goto :err
echo.
echo XONG! Da day len nhanh buoi-5.
pause
exit /b 0
:err
echo.
echo CO LOI - chup man hinh cua so nay gui Claude.
pause
exit /b 1