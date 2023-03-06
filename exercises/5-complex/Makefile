Out.txt : main.exe Makefile
	mono main.exe 

main.exe : main.cs cmath.dll
	mcs -reference:cmath.dll main.cs

cmath.dll : cmath.cs complex.cs
	mcs -target:library -out:cmath.dll $^

.PHONEY : clean
clean: Makefile
	$(RM) *.exe *.dll [Oo]ut* log*