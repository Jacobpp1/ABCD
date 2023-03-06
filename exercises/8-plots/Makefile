Gamma.svg : gamma.gpi gamma.data factorials.data Lngamma.svg Error.svg
	gnuplot $<

Lngamma.svg : lngamma.gpi lngamma.data
	gnuplot $<

Error.svg : error.gpi error.data err_tab.data
	gnuplot $<

gamma.data : main.exe Makefile
	mono main.exe -func:gamma > $@

lngamma.data : main.exe Makefile
	mono main.exe -func:lngamma > $@

error.data : main.exe Makefile
	mono main.exe -func:error > $@

main.exe : main.cs sfuns.dll factorials.data
	mcs -target:exe -out:$@ \
		$(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

sfuns.dll : gamma.cs erf.cs
	mcs -target:library -out:$@ $^

factorials.data : Makefile
	>$@
	echo 0 1 >>$@
	echo 1 1 >>$@
	echo 2 2 >>$@
	echo 3 6 >>$@
	echo 4 24 >>$@
	echo 5 120 >>$@
	echo 6 $$(echo '6*120' | bc) >>$@

main: main.exe sfuns.dll
	mkbundle -o $@ --simple $^

clean :
	rm -f main.exe gamma.data lngamma.data error.data sfuns.dll Gamma.svg Lngamma.svg Error.svg