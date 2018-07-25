package models.services

import java.util.UUID
import javax.inject.{Inject, Singleton}

import com.mohiva.play.silhouette.api.LoginInfo
import com.mohiva.play.silhouette.api.repositories.AuthInfoRepository
import com.mohiva.play.silhouette.api.util.{PasswordHasherRegistry, PasswordInfo}
import com.mohiva.play.silhouette.impl.providers.CredentialsProvider
import com.mohiva.play.silhouette.persistence.daos.DelegableAuthInfoDAO
import models.User
import models.daos.{PasswordDAO, UserDAO}

import scala.concurrent.{ExecutionContext, Future}

trait NekoInitializerService {
  def initializeRoot(): Future[Unit]
}

@Singleton
class NekoInitializerServiceImpl @Inject()(
                                            userDAO: UserDAO,
                                            userService: UserService,
                                            passwordDAO: DelegableAuthInfoDAO[PasswordInfo],
                                            authInfoRepository: AuthInfoRepository,
                                            passwordHasherRegistry: PasswordHasherRegistry
                                          )(
                                            implicit
                                            ex: ExecutionContext
                                          ) extends NekoInitializerService {
  override def initializeRoot() = {
    val loginInfo = LoginInfo(CredentialsProvider.ID, "neko")
    val root = User(
      userID = UUID.randomUUID(),
      loginInfo = loginInfo,
      fullName = "ねこですよろしくおねがいします",
      roles = Seq("neko")
    )
    val authInfo = passwordHasherRegistry.current.hash("neko")
    userService.retrieve(loginInfo).flatMap {
      case Some(_) =>
        Future.successful(())
      case None =>
        for {
          _ <- userService.save(root)
          _ <- authInfoRepository.add(loginInfo, authInfo)
        } yield {
          ()
        }
    }
  }

  initializeRoot()
}