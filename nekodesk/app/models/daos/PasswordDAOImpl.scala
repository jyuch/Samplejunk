package models.daos

import com.mohiva.play.silhouette.api.LoginInfo
import com.mohiva.play.silhouette.api.util.PasswordInfo
import com.mohiva.play.silhouette.persistence.daos.DelegableAuthInfoDAO

import scala.collection.mutable
import scala.concurrent.ExecutionContext.Implicits.global
import scala.concurrent.Future

class PasswordDAOImpl extends PasswordDAO {

  var data: mutable.HashMap[LoginInfo, PasswordInfo] = mutable.HashMap()

  def find(loginInfo: LoginInfo): Future[Option[PasswordInfo]] = {
    data.get(loginInfo) match {
      case Some(l) => println(l)
      case None => println()
    }

    Future.successful(data.get(loginInfo))
  }

  def add(loginInfo: LoginInfo, authInfo: PasswordInfo): Future[PasswordInfo] = {
    println(authInfo)
    data += (loginInfo -> authInfo)
    Future.successful(authInfo)
  }

  def update(loginInfo: LoginInfo, authInfo: PasswordInfo): Future[PasswordInfo] = {
    data += (loginInfo -> authInfo)
    Future.successful(authInfo)
  }

  def save(loginInfo: LoginInfo, authInfo: PasswordInfo): Future[PasswordInfo] = {
    find(loginInfo).flatMap {
      case Some(_) => update(loginInfo, authInfo)
      case None => add(loginInfo, authInfo)
    }
  }

  def remove(loginInfo: LoginInfo): Future[Unit] = {
    data -= loginInfo
    Future.successful(())
  }
}
