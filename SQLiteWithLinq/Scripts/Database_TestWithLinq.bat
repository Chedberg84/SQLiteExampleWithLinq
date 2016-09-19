REM Delete Database
REM DEL "TestWithLinq.db3" /Q

REM Run Setup
sqlite3.exe .read SQLite_Commands.txt

REM End Of File
PAUSE
