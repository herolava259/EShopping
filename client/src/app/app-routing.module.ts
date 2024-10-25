import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProductDetailsComponent } from './store/product-details/product-details.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { UnAuthenticatedComponent } from './core/un-authenticated/un-authenticated.component';
import { SigninRedirectCallbackComponent } from './account/signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './account/signout-redirect-callback/signout-redirect-callback.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    data: {breadcrumb: 'Home'}
  },
  {
    path: 'server-error',
    component: ServerErrorComponent
  },
  {
    path: 'not-found',
    component: NotFoundComponent
  },
  {
    path: 'un-authenticated',
    component: UnAuthenticatedComponent
  },
  {
    path: 'store',
    loadChildren: () => import('./store/store.module').then(mod => mod.StoreModule),
    data: {breadcrumb:'Store'}
  },
  {
    path: 'basket',
    loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule),
    data: { breadcrumb: 'Basket'}
  },
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule),
    data: { breadcrumb: {skip: true} }
  },
  {
    path: 'store/:id',
    component: ProductDetailsComponent
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  },
  { 
    path: 'signin-callback', 
    component: SigninRedirectCallbackComponent 
  },
  { 
    path: 'signout-callback', 
    component: SignoutRedirectCallbackComponent 
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
