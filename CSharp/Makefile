Out.txt : main.exe
	mono main.exe > Out.txt

main1.exe : main.cs static_hello.cs static_world.cs
	mcs main.cs static_hello.cs static_world.cs -target:exe -out:main1.exe

hello.dll : static_hello.cs static_world.cs
	mcs static_hello.cs static_world.cs -target:library -out:hello.dll

main.exe : main.cs hello.dll
	mcs main.cs -reference:hello.dll -target:exe -out:main.exe
