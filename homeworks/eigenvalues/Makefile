Out.txt : main.exe Makefile fixed_dr.svg fixed_rmax.svg
	mono main.exe -rmax:10 -dr:0.3 > Out.txt

main.exe : main.cs Makefile matrix.dll funcs.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

funcs.dll : funcs.cs
	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll

fixed_dr.svg : fixed_dr.gpi fixed_dr.data
	gnuplot $<

fixed_rmax.svg : fixed_rmax.gpi fixed_rmax.data
	gnuplot $<

eigenfuncs.svg : eigenfuncs.gpi eigenfuncs.data
	gnuplot $<