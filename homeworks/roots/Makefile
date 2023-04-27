default : Out.txt wvf.svg rmin_conv.svg rmax_conv.svg abs_acc.svg eps_acc.svg

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs matrix.dll funcs.dll genlist.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

funcs.dll : funcs.cs genlist.dll matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

wvf.svg : wavefunction.gpi wavefunction.data
	gnuplot $<
	
rmin_conv.svg : rmin_conv.gpi rmin_conv.data
	gnuplot $<
rmax_conv.svg : rmax_conv.gpi rmax_conv.data
	gnuplot $<
abs_acc.svg : abs_acc.gpi abs_acc_conv.data
	gnuplot $<
eps_acc.svg : eps_acc.gpi eps_acc_conv.data
	gnuplot $<

clean :
	$(RM) *.exe *.data *.svg