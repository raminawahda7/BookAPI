import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { PublisherResource } from "../Resources/PublisherResource";
import { Publisher } from './../models/Publisher';

@Injectable({
    providedIn:'root'
})

export class PublisherService{
    private baseUrl:string = environment.baseUrl+'api/';
    
    constructor(private http:HttpClient){}

    public getPublishers():Observable<PublisherResource[]>{
        return this.http.get<PublisherResource[]>(this.baseUrl+'publishers');
    }
    public getPublisherById(id:number):Observable<PublisherResource>{
        return this.http.get<PublisherResource>(this.baseUrl+'publishers/'+id)
    }
    public addPublisher(publihser:Publisher){
        return this.http.post(this.baseUrl+'publishers',publihser);
    }
    public updatePublisher(id:number,publisher:Publisher){
        return this.http.put(this.baseUrl+'publishers/'+id,publisher)
    }
    public deletePublisher(id:number){
        return this.http.delete(this.baseUrl+'publishers/'+id);
    }
    // TODO Add filter by book and by author too.

}