import axiosInstance from "./axiosInstance";

export const authService = {
    //Post /api/logjn  
    login : (Credentials)=> axiosInstance.post('/Authentication',Credentials), 
};
 
 