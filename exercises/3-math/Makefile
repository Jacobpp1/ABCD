Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs sfuns.dll
	mcs main.cs -reference:sfuns.dll -target:exe -out:main.exe

sfuns.dll : sfuns.cs main.cs
	mcs sfuns.cs -target:library -out:sfuns.dll