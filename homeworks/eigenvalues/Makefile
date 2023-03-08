Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs Makefile matrix.dll funcs.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

funcs.dll : funcs.cs
	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll