default : Out.txt opt_N.svg func_calls.svg iterations.svg vary_N_varerr.svg vary_N_meanerr.svg

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

opt_N.svg : opt_N.gpi optimum_N_var.data
	gnuplot $<

func_calls.svg : func_calls.gpi optimum_N_var.data
	gnuplot $<

iterations.svg : iterations.gpi optimum_N_var.data
	gnuplot $<

vary_N_varerr.svg : N_vary_varerr.gpi vary_N_var.data
	gnuplot $<
vary_N_meanerr.svg : N_vary_meanerr.gpi vary_N_var.data
	gnuplot $<

clean :
	$(RM) *.exe *.svg *.data *.gif