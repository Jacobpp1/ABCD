default : Out.txt Makefile eigenfuns.svg fixed_dr.svg fixed_rmax.svg

Out.txt : main.exe Makefile
	mono main.exe -rmax:10 -dr:0.3 > Out.txt

main.exe : main.cs Makefile matrix.dll funcs.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

funcs.dll : funcs.cs
	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll

fixed_dr.svg : fixed_dr.gpi fixed_dr.data
	gnuplot $<

fixed_rmax.svg : fixed_rmax.gpi fixed_rmax.data
	gnuplot $<

eigenfuns.svg : eigenfuns.gpi eigenfuns_n1.data eigenfuns_n2.data eigenfuns_n3.data
	gnuplot $<

clean :
	$(RM) *.svg *.data