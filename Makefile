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
		src/Microsoft.Ccr.Core/IterativeTasks.cs \
		src/Microsoft.Ccr.Core/Port.cs \
		src/Microsoft.Ccr.Core/PortExtensions.cs \
		src/Microsoft.Ccr.Core/PortNotFoundException.cs \
		src/Microsoft.Ccr.Core/PortSet.cs \
		src/Microsoft.Ccr.Core/PortSet.generated.cs \
		src/Microsoft.Ccr.Core/Receiver.cs \
		src/Microsoft.Ccr.Core/SuccessResult.cs \
		src/Microsoft.Ccr.Core/SuccessFailurePort.cs \
		src/Microsoft.Ccr.Core/Tasks.cs \
		src/Microsoft.Ccr.Core/TaskCommon.cs \
		src/Microsoft.Ccr.Core/TaskExecutionPolicy.cs \
		src/Microsoft.Ccr.Core/Tuple.cs \
		src/Microsoft.Ccr.Core/VariableArgumentTask.cs \
		src/MonoTODOAttribute.cs \

TEST_FILES = test/Microsoft.Ccr.Core/ArbiterTest.cs \
			 test/Microsoft.Ccr.Core/IterativeTaskTest.cs \
			 test/Microsoft.Ccr.Core/PortExtensionsTest.cs \
			 test/Microsoft.Ccr.Core/PortTest.cs \
			 test/Microsoft.Ccr.Core/PortSetTest.cs \
			 test/Microsoft.Ccr.Core/DispatcherTest.cs \
			 test/Microsoft.Ccr.Core/DispatcherQueueTest.cs \
			 test/Microsoft.Ccr.Core/ReceiverTest.cs \
			 test/Microsoft.Ccr.Core/SuccessResultTest.cs \
			 test/Microsoft.Ccr.Core/TaskTest.cs \
			 test/Microsoft.Ccr.Core/VariableArgumentTaskTest.cs \
			 test/Microsoft.Ccr.Core.Arbiters/PortElementTest.cs \

src/Microsoft.Ccr.Core/PortSet.generated.cs: src/Microsoft.Ccr.Core/make_portset.rb
	@ruby src/Microsoft.Ccr.Core/make_portset.rb > src/Microsoft.Ccr.Core/PortSet.generated.cs

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