default : Out.txt A_fig.svg

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

A_fig.svg : A_fig.gpi
	gnuplot $<

clean :
	$(RM) *.exe *.svg *.data