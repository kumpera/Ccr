FILES = src/Assembly/AssemblyInfo.cs \
		src/Microsoft.Ccr.Core/Arbiter.cs \
		src/Microsoft.Ccr.Core.Arbiters/ArbiterTaskState.cs \
		src/Microsoft.Ccr.Core.Arbiters/IArbiterTask.cs \
		src/Microsoft.Ccr.Core.Arbiters/IPortArbiterAccess.cs \
		src/Microsoft.Ccr.Core.Arbiters/IPortElement.cs \
		src/Microsoft.Ccr.Core.Arbiters/PortElement.cs \
		src/Microsoft.Ccr.Core.Arbiters/PortMode.cs \
		src/Microsoft.Ccr.Core.Arbiters/PortSetMode.cs \
		src/Microsoft.Ccr.Core.Arbiters/ReceiverTask.cs \
		src/Microsoft.Ccr.Core.Arbiters/ReceiverTaskState.cs \
		src/Microsoft.Ccr.Core/Dispatcher.cs \
		src/Microsoft.Ccr.Core/DispatcherOptions.cs \
		src/Microsoft.Ccr.Core/DispatcherQueue.cs \
		src/Microsoft.Ccr.Core/EmptyValue.cs \
		src/Microsoft.Ccr.Core/Handlers.cs \
		src/Microsoft.Ccr.Core/InterleaveReceivers.cs \
		src/Microsoft.Ccr.Core/ICausality.cs \
		src/Microsoft.Ccr.Core/IPort.cs \
		src/Microsoft.Ccr.Core/IPortReceive.cs \
		src/Microsoft.Ccr.Core/IPortSet.cs \
		src/Microsoft.Ccr.Core/ITask.cs \
		src/Microsoft.Ccr.Core/Port.cs \
		src/Microsoft.Ccr.Core/PortNotFoundException.cs \
		src/Microsoft.Ccr.Core/PortSet.cs \
		src/Microsoft.Ccr.Core/Receiver.cs \
		src/Microsoft.Ccr.Core/TaskCommon.cs \
		src/Microsoft.Ccr.Core/TaskExecutionPolicy.cs \
		src/Microsoft.Ccr.Core/Tuple.cs \
		src/MonoTODOAttribute.cs \

TEST_FILES = test/Microsoft.Ccr.Core/PortTest.cs \
			 test/Microsoft.Ccr.Core/PortSetTest.cs \
			 test/Microsoft.Ccr.Core/DispatcherQueueTest.cs \
			 test/Microsoft.Ccr.Core.Arbiters/PortElementTest.cs \


all: deps Microsoft.Ccr.dll Microsoft.Ccr_test.dll


Microsoft.Ccr.Core.dll: $(FILES)
	gmcs -debug -target:library -out:Microsoft.Ccr.Core.dll $(FILES)

Microsoft.Ccr.Core_test.dll: Microsoft.Ccr.Core.dll $(TEST_FILES)
	gmcs -debug -target:library -out:Microsoft.Ccr.Core_test.dll -r:Microsoft.Ccr.Core.dll -r:bin/nunit.framework.dll  -r:bin/nunit.core.dll $(TEST_FILES)  

compile: Microsoft.Ccr.Core.dll Microsoft.Ccr.Core_test.dll
	@echo done

run-test: Microsoft.Ccr.Core_test.dll
	 nunit-console2 Microsoft.Ccr.Core_test.dll

refresh_src_files:
	@find src/ | grep "\\.cs" | sort > ccr.dll.sources
	@find test/ | grep "\\.cs" | sort > ccr_test.dll.sources


.PHONY: refresh_src_files run_test