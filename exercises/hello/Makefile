out. txt : hello.exe
	mono hello.exe > Out.txt

hello.exe : hello.cs
	mcs -out:hello.exe hello.cs

.Phoney:clean
clean:
	rm --force Out.txt hello.exe
