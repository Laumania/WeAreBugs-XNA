##WeAreBugs-XNA

WeAreBugs-XNA is a XNA port of the game "We Are Bugs" by [Jeff Weber](http://www.farseergames.com/blog/2009/4/30/we-are-bugs-refactored-and-source-released.html)


This project includes two versions of "We Are Bugs".

- **BugsSL** - A direct port of Jeff's Silverlight(browser) version to a Silverlight version that runs and works on Windows Phone.

- **BugsXNA** - A XNA version of the game.

It's in the BugsXNA project that I have put the work. 
The goal is to keep the source(XNA) as close to the original(Silverlight) one, but still being true to how XNA do things.

If you grab the source from the 'master' branch you will see that port. A as-simple-as-possible port of the game. 
It's still not a 100 % port, but close to. 
What I'm currently missing (you can see these as 'issues' too) is:
- Turning the EnemyBug red as it gets closer to the Bug (it gets excited!)
- Making the game "time-based" instead of "frame/tick-based" as it is now
- Adding the "MotionTrail" the BugModel, so it looks like it goes faaassstt!

If you want to play around with the code, please fork and implement one or two of these issues and send me a Pull Request :)


Bonus -> Optimization branch:
As a little bonus, I have done a "Optimization" branch, where I have added a few minor improvements to the game. These are mostly related to how content (the graphics) are preprocessed and handled.
This can be used as inspiration.

If you have questions, feel free to contact my via GitHub or my blog - http://www.laumania.net

Enjoy & stay tuned :)