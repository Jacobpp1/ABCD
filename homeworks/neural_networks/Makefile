default : Out.txt fit_A.svg fit_B.svg

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

fit_A.svg : fit_A.gpi
	gnuplot $<
fit_B.svg : fit_B.gpi
	gnuplot $<