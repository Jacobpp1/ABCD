Out.txt : main.exe gnuplot_fit.svg self_fit.svg self_fit_C.svg
	mono main.exe > Out.txt

gnuplot_fit.svg : gnuplot_fit.gpi
	gnuplot $<

self_fit.svg : self_fit.gpi
	gnuplot $<

self_fit_C.svg : self_fit_C.gpi
	gnuplot $<

main.exe : main.cs Makefile funcs.dll matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

funcs.dll : funcs.cs
	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll

clean : 
	$(RM) *.exe *.svg *.data