default : Out.txt A_errs.svg B_errs.svg

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

A_errs.svg : A_err_plot.gpi A_errs.data
	gnuplot $<

B_errs.svg : B_err_plot.gpi A_errs.data B_errs.data
	gnuplot $<

clean :
	$(RM) *.exe