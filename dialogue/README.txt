base unity project for dialogue system using ink

any font, graphics, or other visuals can be substituted in - fine-tuning the scaling will be necessary to get it looking right

improvements on implementation:
frame for left-right decision toggle gets confused, probably an animation issue
decision frame also sometimes sticks on the right option, but pressing right unsticks it into a left->right animation. also an animation issue
mashing spacebar makes multiple lines of text smash together into jibberish, a small delay between accepted inputs would fix this
setting shift is instantaneous, so if text & black-fade isn't intentionally designed it can result in some unintended visuals. could solve or just work around
for longer projects where proofreading is annoying, it would be nice if longer lines that clip out of the box were automatically split, while newlines are interpreted the same way
should do dictionary entries (rather than hardcoding) for characters if more than one or two are implemented into the project.