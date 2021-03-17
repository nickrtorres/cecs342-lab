#include <cassert>
#include <iostream>
/*
 * 2021-03-10
 */

struct Wrapper {
  Wrapper(int x) : mX(x) {}

  Wrapper(const Wrapper &other) {
    std::cout << "copy ctor other ..." << std::endl;
    this->mX = other.mX;
  }

  Wrapper &operator=(const Wrapper &other) {
    std::cout << "copy assignment other ..." << std::endl;
    this->mX = other.mX;
    return *this;
  }

  int X() const { return mX; }

  void SetX(int x) { this->mX = x; }

private:
  int mX;
};

/*
 * The structure of a closure:
 *   - callable block
 *   - executing environment (ie. state that the function will execute with)
 */
int main() {
  {
    /* want function to add two numbers together
     * similar to F# style (fun a b -> a + b) */
    auto add = [](int a, int b) { return a + b; };
    assert(3 == add(1, 2));
  }

  {
    /* want to take the argument a and add it to x within the body of addToX */
    int x = 42;
    auto addToX = [&x](int a) { return a + x; };
    auto result1 = addToX(10); /* what is the value of result1 */
    assert(52 == result1);

    /* what kind of binding does addToX use? */
    x = 100;
    auto result2 = addToX(10); /* what is the value of result2 */
    assert(110 == result2);
  }

  {
    int x = 42;
    auto addToX = [x](int a) { return a + x; };

    auto result1 = addToX(10); /* what is the value of result1 */
    assert(52 == result1);

    x = 100;
    auto result2 = addToX(10); /* what is the value of result2 */
    assert(52 == result1);
  }

  {
    Wrapper w(42);
    auto addToXByVal = [w](int a) { return a + w.X(); };
    addToXByVal(10);
  }

  {
    Wrapper w(42);
    /* are we making a copy of w? */
    auto addToXByRef = [&w](int a) { return a + w.X(); };
    addToXByRef(10);
  }

  {
    Wrapper w(42);
    auto updateW = [w](int a) mutable { w.SetX(a); };
    updateW(10);
    assert(42 == w.X());
  }

  {
    Wrapper w(42); /* w is the referent */
    auto updateW = [&w](int a) { w.SetX(a); };
    updateW(10);
    /* what is the value of w.X() at this point? */
    /* Yes = 42; No = 10 */
    assert(10 == w.X());
  }

  /*
   * - How do we implement closures in C++ manually?
   *
   * Split these into two groups: by value and by reference
   *
   * How do we implement a closure that can capture by value?
   *
   * What do we need?
   * - state
   * - call
   *
   * C++:
   * ----
   * - primitive types: int, float, double, char
   * - functions
   * - classes and structs
   */

  /* goal:
   * int x = 42;
   * auto addToX = [x](int a){ return a + x; }
   */
  class ValueClosure {
    int mValue;

  public:
    ValueClosure(int value) : mValue(value) {}

    /* we want something that will take, as a parameter, an integer and return
     * the sum of our state and that integer */
    int Sum(int x) { return mValue + x; }

    /* parenthesis are operators */
    int operator()(int x) const { return mValue + x; }
  };

  int x = 42;
  ValueClosure closure(x);
  assert(142 == closure.Sum(100));
  assert(142 == closure(100));

  /* will we see the same value? */
  x = 100;
  assert(142 == closure(100));
  x = 101;
  assert(142 == closure(100));
  x = 102;
  assert(142 == closure(100));
  x = 103;
  assert(142 == closure(100));
  x = 104;
  assert(142 == closure(100));
  x = 105;
  assert(142 == closure(100));

  /* goal:
   * int x = 42;
   * auto addToX = [x](int a){ return a + x; }
   */
  class RefClosure {
    int &mRef;

  public:
    RefClosure(int &ref) : mRef(ref) {}

    /* we want something that will take, as a parameter, an integer and return
     * the sum of our state and that integer */
    int Sum(int x) { return mRef + x; }

    /* parenthesis are operators */
    int operator()(int x) const { return mRef + x; }
  };

  int y = 42;
  RefClosure rclosure(y);
  assert(142 == rclosure(100));

  y = 100;
  assert(200 == rclosure(100));

  /*
   * - What can go wrong when you capture by reference?
   *   Dangling references
   *   Invalid pointers
   *   RAII
   */

  return 0;
}
