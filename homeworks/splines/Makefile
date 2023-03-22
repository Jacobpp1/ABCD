Out.txt : main.exe Makefile A_test.svg
	mono main.exe > Out.txt

main.exe : main.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

A_test.svg : A_test.gpi
	gnuplot $<