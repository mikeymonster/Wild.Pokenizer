# Clear everything
rm(list=ls())

# install.packages("imager")

# install.packages("tidyverse")
# install.packages(c("nycflights13", "gapminder", "Lahman"))

library(ggplot2)
mpg
ggplot(data = mpg) +
  geom_point(mapping = aes(x = displ, y = hwy))

ggplot(mpg) +
  geom_point(aes(displ, hwy, color=model))

ggplot(mpg) +
  geom_point(aes(displ, hwy, color=displ))

df_history <- as.data.frame(history)
df_history
summary(history)
ggplot(df_history) +
  geom_point(mapping = aes(x = epoch, y = metric, color = metric, )) +
  geom_smooth(mapping = aes(x = epoch, y = metric))


df_history <- as.data.frame(history)
df_history
summary(history)
ggplot(df_history) +
  geom_point(mapping = aes(x = epoch, y = value, color = metric, )) +
  geom_line(mapping = aes(x = epoch, y = value, color = metric, ))
#  geom_smooth(mapping = aes(x = epoch, y = value, color = metric))


library(tensorflow)
#install_tensorflow()
sess = tf$Session()
hello <- tf$constant('Hello, TensorFlow!')
sess$run(hello)

condaEnvironmentName <- "tf-gpu"
use_condaenv(condaEnvironmentName)
sess = tf$Session()

