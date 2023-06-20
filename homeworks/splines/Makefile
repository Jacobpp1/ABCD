default : Out.txt A_fig.svg B_fig.svg

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

A_fig.svg : A_fig.gpi A_orig.data A_test.data
	gnuplot $<

B_fig.svg : B_fig.gpi A_orig.data B_test.data
	gnuplot $<

clean :
	$(RM) *.exe *.svg *.data