/*
 * File:
 *   MinHeapTest.cs
 * 
 * Description:
 *   This file contains a class used to test the basic
 *   functionality of a minimum heap data structure. To
 *   use it, just create an empty game object in a scene
 *   and attach this script, then examine the heap provided
 *   by the script in the inspector. It also contains a
 *   static class with metrics used by the test class.
 */

using UnityEngine;

/*
 * Class:
 *   MinHeapTest
 * 
 * Description:
 *   This is a class designed to test the minimum heap
 *   data structure. It will test
 */
public class MinHeapTest : MonoBehaviour
{
    // Serialization allows us to see the members of the
    // heap in the inspector. This is the min heap that
    // we will be testing.
    [SerializeField] MinHeap testHeap;

	/*
     * Method:
     *   Start
     * 
     * Description:
     *   Creates and tests a minimum heap data structure.
     *   The tests will display their status in the console.
     */
	private void Start()
    {
        // Create and test a min heap.
        TestHeap();
	}

    /*
     * Method:
     *   TestHeap
     * 
     * Description:
     *   Tests the min heap structure.
     */
    private void TestHeap()
    {
        // Count of the number of successful tests.
        int testsPassed = 0;

        // Create a heap to test.
        testHeap = new MinHeap(MinHeapTestConstants.TEST_HEAP_SIZE);

        /*
         * TEST 1:
         *   See if the heap is initally empty. Upon
         *   creation, before inserting any nodes, a
         *   min heap should be empty.
         */
        if (testHeap.IsEmpty())
        {
            PrintTestResults("Empty Heap");
            testsPassed++;
        }
        else
        {
            PrintTestResults("Empty Heap", "Failed: heap was not initially empty.");
        }

        /*
         * TEST 2:
         *   See if the heap contains any valid data.
         *   As we haven't added any nodes yet, it shouldn't.
         *   This test also makes sure the heap doesn't explode
         *   when we ask if it contains a vertex larger than its
         *   size.
         */
        for (int index = 0; index <= MinHeapTestConstants.TEST_HEAP_SIZE; index++)
        {
            // See if the current index is in the heap. If it is, break out
            // of the loop and display a failure.
            if (testHeap.Contains(index))
            {
                // Print the failure.
                PrintTestResults("Negative Contains", "Failed: " + index +
                    " incorrectly returned true.");

                // Stop testing.
                break;
            }

            // See if the test was successful.
            if (index == MinHeapTestConstants.TEST_HEAP_SIZE)
            {
                PrintTestResults("Negative Contains");
                testsPassed++;
            }
        }

        /*
         * TEST 3:
         *   Attempt to extract the minimum value.
         *   Since we have no data, this should just return null.
         */
        if (testHeap.ExtractMin() == MinHeap.InvalidExtract)
        {
            PrintTestResults("Empty ExtractMin");
            testsPassed++;
        }
        else
        {
            PrintTestResults("Empty ExtractMin", "Failed: extracted a value from an empty heap.");
        }

        /*
         * TEST 4:
         *   Add several nodes and then extract mins until the
         *   heap is empty. Make sure that we do not add the nodes
         *   in the order they will be extracted, because that would
         *   not test the heapify function.
         */
        if (RandomHeapTest(testHeap))
        {
            testsPassed++;
        }

        if (testsPassed == MinHeapTestConstants.NUM_TESTS)
        {
            Debug.Log("All min heap tests passed!");
        }
        
    }

    /*
     * Method:
     *  RandomHeapTest
     *
     * Description:
     *   Takes random values and inserts them into the heap. Then, extracts
     *   mins and tests to make sure the values extracted are in the correct
     *   order.
     */
    private bool RandomHeapTest(MinHeap heap)
    {
        // Create a reference to the random data we pass into the heap.
        float[] heapData = new float[MinHeapTestConstants.TEST_HEAP_SIZE];

        for (int index = 0; index < MinHeapTestConstants.TEST_HEAP_SIZE; index++)
        {
            // Add a new random value to the heap. We also save the value
            // so that when we extract the mins later we know if the extracted
            // key was really the minimum.
            float randomVal = Random.Range(0, 100);
            heap.InsertMinHeapNode(index, randomVal);
            heapData[index] = randomVal;
        }

        // Get a refernce to the previous and current mins extracted.
        int lastExtracted = heap.ExtractMin();
        int currentExtracted = heap.ExtractMin();

        // Extract all the nodes.
        while (currentExtracted != MinHeap.InvalidExtract)
        {
            if (heapData[currentExtracted] < heapData[lastExtracted])
            {
                // We extracted a smaller value than the previous time.
                // This is a failure.
                PrintTestResults("Random Heap", "Failed: extracted " +
                    heapData[lastExtracted] + " before " +
                    heapData[currentExtracted] + ".");

                return false;
            }

            // Continue extracting until the heap is empty.
            lastExtracted = currentExtracted;
            currentExtracted = heap.ExtractMin();
        }

        // Extracted all nodes correctly.
        PrintTestResults("Random Heap");

        return true;
    }

    /*
     * Method:
     *   PrintTestResults
     * 
     * Description:
     *   Prints the results of the current test to the console.
     */
    private void PrintTestResults(string testName, string message = "Passed!")
    {
        // Print the test results, formatted nicely.
        Debug.Log(testName + " test: " + message);
    }
	
}

/*
 * Class:
 *   MinHeapTestConstants
 * 
 * Description:
 *   This is a static class that contains constants used
 *   in testing the functionality of the min heap data
 *   structure.
 */

public static class MinHeapTestConstants
{
    // Define the size of the min heap to test.
    // This is the maximum number of nodes that
    // the test heap can support.
    public static int TEST_HEAP_SIZE = 8;

    // Define the total number of tests we will run.
    public static int NUM_TESTS = 4;
}
