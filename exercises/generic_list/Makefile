Out.txt : main.exe Makefile 
	mono $< 1>$@ 2> log < numbers.txt

main.exe : main.cs 
	mcs -target:exe -out:$@ \
		$(filter %.cs,$^)

clean:
	$(RM) *.exe *.dll [Oo]out* *log
