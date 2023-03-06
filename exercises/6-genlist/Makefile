Out.txt : main.exe Makefile input.txt
	mono main.exe <input.txt > $@

main.exe : main.cs genlist.dll
	mcs -target:exe -out:$@ \
		$(addprefix -reference:,$(filter %.dll,$^)) \
		$(filter %.cs,$^)

genlist.dll : genlist.cs
	mcs -target:library -out:$@ $<

clean:
	$(RM) *.exe *.dll [Oo]out* *log