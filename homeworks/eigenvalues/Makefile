default : Out.txt fixed_dr.svg fixed_rmax.svg eigenfuns.svg fixed_dr.data \
fixed_rmax.data eigenfuns_dr0.1_rmax5.data eigenfuns_dr0.2_rmax10.data eigenfuns_dr0.3_rmax12.data

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

eigenfuns.svg : eigenfuns.gpi eigenfuns_dr0.1_rmax5.data eigenfuns_dr0.2_rmax10.data eigenfuns_dr0.3_rmax12.data
	gnuplot $<

clean :
	$(RM) *.svg *.data