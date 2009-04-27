FILES = src/Assembly/AssemblyInfo.cs \
		src/Microsoft.Ccr.Core/Arbiter.cs \
		src/Microsoft.Ccr.Core.Arbiters/IPortArbiterAccess.cs \
		src/Microsoft.Ccr.Core.Arbiters/IPortElement.cs \
		src/Microsoft.Ccr.Core.Arbiters/PortMode.cs \
		src/Microsoft.Ccr.Core.Arbiters/ReceiverTask.cs \
		src/Microsoft.Ccr.Core/IPort.cs \
		src/Microsoft.Ccr.Core/IPortReceive.cs \
		src/Microsoft.Ccr.Core/Port.cs \

TEST_FILES = test/Microsoft.Ccr.Core/PortTest.cs

all: deps Microsoft.Ccr.dll Microsoft.Ccr_test.dll


Microsoft.Ccr.Core.dll: $(FILES)
	gmcs -debug -target:library -out:Microsoft.Ccr.Core.dll $(FILES)

Microsoft.Ccr.Core_test.dll: Microsoft.Ccr.Core.dll $(TEST_FILES)
	gmcs -debug -target:library -out:Microsoft.Ccr.Core_test.dll -r:Microsoft.Ccr.Core.dll -r:bin/nunit.framework.dll  -r:bin/nunit.core.dll $(TEST_FILES)  

compile: Microsoft.Ccr.Core.dll Microsoft.Ccr.Core_test.dll
	@echo done

run-test: Microsoft.Ccr.Core_test.dll
	 mono --debug bin/nunit-console.exe --noshadow Microsoft.Ccr.Core_test.dll

refresh_src_files:
	@find src/ | grep "\\.cs" | sort > ccr.dll.sources
	@find test/ | grep "\\.cs" | sort > ccr_test.dll.sources


.PHONY: refresh_src_files run_test