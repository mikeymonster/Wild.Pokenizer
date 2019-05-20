# Clear everything
rm(list=ls())

# install.packages("imager")

install.packages("tidyverse")
install.packages(c("nycflights13", "gapminder", "Lahman"))

library(ggplot2)
mpg
ggplot(data = mpg) +
  geom_point(mapping = aes(x = displ, y = hwy))

ggplot(mpg) +
  geom_point(aes(displ, hwy, color=model))

ggplot(mpg) +
  geom_point(aes(displ, hwy, color=displ))
