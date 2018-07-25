package modules

import com.google.inject.AbstractModule
import models.services.{NekoInitializerService, NekoInitializerServiceImpl}
import net.codingwell.scalaguice.ScalaModule

class NekoInitializerModule extends  AbstractModule with ScalaModule {

   def configure() {
    bind[NekoInitializerService].to[NekoInitializerServiceImpl].asEagerSingleton()
  }
}
