import { Component, OnInit } from '@angular/core';
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { Router, ActivatedRoute, NavigationEnd, Params, PRIMARY_OUTLET } from "@angular/router";
import { filter } from 'rxjs/operators';

interface IBreadcrumb {
  label: string;  
  url: string;
  params?: Params;
  isActive?: Boolean;
  isHome?: Boolean;
}

@Component({
  selector: 'breadcrumb',
  templateUrl: './breadcrumb.component.html'
})
export class BreadcrumbComponent implements OnInit {  

  public breadcrumbs: IBreadcrumb[];

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    const ROUTE_DATA_BREADCRUMB: string = "breadcrumb";

    //subscribe to the NavigationEnd event
    //this.router.events
    //  .filter(event => event instanceof NavigationEnd)
    //  .distinctUntilChanged()
    //  .map(event => this.buildBreadCrumb(this.activatedRoute.root));

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.breadcrumbs = this.buildBreadCrumb(this.activatedRoute.root)      
    });
  }

  buildBreadCrumb(route: ActivatedRoute, url: string = '',
    breadcrumbs: Array<IBreadcrumb> = []): Array<IBreadcrumb> {

    //If no routeConfig is avalailable we are on the root path
    const label = route.routeConfig ? route.routeConfig.data['breadcrumb'] : 'Home';
    const path = route.routeConfig ? route.routeConfig.path : '';
    
    //In the routeConfig the complete path is not available, 
    //so we rebuild it each time
    let nextUrl = `${url}${path}/`;
    const isHome = (route.routeConfig ? false : true);

    let params = Object.keys(route.snapshot.params);
    for (var param of params) {
      nextUrl = nextUrl.replace(':' + param, route.snapshot.params[param])
    }
    const breadcrumb = <IBreadcrumb>{
      label: label,
      url: nextUrl,
      isHome: isHome
    };
    const newBreadcrumbs = [...breadcrumbs, breadcrumb];    
    if (route.firstChild && route.firstChild.routeConfig.path) {
      //If we are not on our current path yet, 
      //there will be more children to look after, to build our breadcumb
      return this.buildBreadCrumb(route.firstChild, nextUrl, newBreadcrumbs);
    }
    newBreadcrumbs[newBreadcrumbs.length - 1].isActive = true;
    return newBreadcrumbs;
  }

}
